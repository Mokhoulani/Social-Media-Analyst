import {
    catchError,
    Observable,
    shareReplay,
    Subject,
    switchMap,
    take,
    throwError,
    timeout,
    TimeoutError,
} from 'rxjs'
import z from 'zod'
import { getStoredRefreshToken$ } from '../../utils/jwt-utils'
import { store } from '../store'
import { AuthActions } from './actions'
import { authSelectors } from './selectors'

export const signUpSchema = z.object({
    firstName: z.string().min(1, { message: 'First name is required' }),
    lastName: z.string().min(1, { message: 'Last name is required' }),
    email: z.string().email({ message: 'Invalid email address' }),
    password: z
        .string()
        .min(6, { message: 'Password must be at least 6 characters' })
        .refine((val) => /[A-Z]/.test(val), {
            message: 'Password must contain at least one uppercase letter',
        })
        .refine((val) => /[a-z]/.test(val), {
            message: 'Password must contain at least one lowercase letter',
        })
        .refine((val) => /[0-9]/.test(val), {
            message: 'Password must contain at least one digit',
        })
        .refine((val) => /[!@#$%^&*(),.?":{}|<>_\-\\/[\]=+~`]/.test(val), {
            message: 'Password must contain at least one special character',
        }),
})

export type SignUpForm = z.infer<typeof signUpSchema>

let sharedRefresh$: Observable<void> | null = null
/**
 * AuthFacade provides a simplified interface for components to interact with auth state
 * This abstracts away the Redux implementation details
 */
export const AuthFacade = {
    /**
     * Login with email and password
     */
    login(email: string, password: string) {
        const result$ = new Subject<boolean>()

        // Dispatch the login request
        store.dispatch(AuthActions.loginRequest({ email, password }))

        // Listen for success or failure using store.subscribe
        const unsubscribe = store.subscribe(() => {
            const state = store.getState()
            const error = authSelectors.selectError(state)
            const isAuthenticated = authSelectors.selectIsAuthenticated(state)
            if (isAuthenticated) {
                result$.next(true)
                result$.complete()
                unsubscribe()
            } else if (error) {
                result$.error('Login failed')
                result$.complete()
                unsubscribe()
            }
        })

        return result$.asObservable()
    },
    /**
     * Sign up a new user
     */
    signUp(form: SignUpForm) {
        const result$ = new Subject<boolean>()

        store.dispatch(
            AuthActions.signUpRequest({
                firstName: form.firstName,
                lastName: form.lastName,
                email: form.email,
                password: form.password,
            })
        )

        const unsubscribe = store.subscribe(() => {
            const state = store.getState()
            const error = authSelectors.selectError(state)
            const isAuthenticated = authSelectors.selectIsAuthenticated(state)

            if (isAuthenticated) {
                result$.next(true)
                result$.complete()
                unsubscribe()
            } else if (error) {
                result$.error('Signup failed')
                result$.complete()
                unsubscribe()
            }
        })

        return result$.asObservable()
    },

    /**
     * Refresh the access token using the refresh token
     */
    refreshToken: () => {
        const refreshToken = authSelectors.selectRefreshToken(store.getState())
        if (refreshToken) {
            store.dispatch(AuthActions.refreshTokenRequest({ refreshToken }))
        } else {
            console.error('No refresh token available')
        }
    },

    /**
     * Logout the current user
     */
    logout: () => {
        const accessToken = authSelectors.selectAccessToken(store.getState())
        store.dispatch(AuthActions.logout({ token: accessToken || '' }))
    },

    /**
     * Check if the user is authenticated
     */
    isAuthenticated: () => {
        return authSelectors.selectIsAuthenticated(store.getState())
    },

    /**
     * Get the current access token
     */
    getAccessToken: () => {
        return authSelectors.selectAccessToken(store.getState())
    },

    /**
     * Get the current auth error (if any)
     */
    getError: () => {
        return authSelectors.selectError(store.getState())
    },

    /**
     * Check if an auth operation is in progress
     */
    isLoading: () => {
        return authSelectors.selectLoading(store.getState())
    },

    /**
     * Refresh the access token using the refresh token
     */
    refreshToken$: (): Observable<void> => {
        if (sharedRefresh$) {
            return sharedRefresh$
        }

        sharedRefresh$ = new Observable<string | null>((subscriber) => {
            const state = store.getState()
            const refreshToken = authSelectors.selectRefreshToken(state)

            if (refreshToken) {
                subscriber.next(refreshToken)
                subscriber.complete()
            } else {
                // Fallback: try getting from SecureStore
                getStoredRefreshToken$().subscribe({
                    next: (storedToken) => {
                        subscriber.next(storedToken)
                        subscriber.complete()
                    },
                    error: (err) => subscriber.error(err),
                })
            }
        }).pipe(
            switchMap((refreshToken) => {
                if (!refreshToken) {
                    return throwError(
                        () => new Error('No refresh token available')
                    )
                }

                return new Observable<void>((subscriber) => {
                    const unsubscribe = store.subscribe(() => {
                        const state = store.getState()
                        const token = authSelectors.selectAccessToken(state)
                        const error = authSelectors.selectError(state)

                        if (token) {
                            subscriber.next()
                            subscriber.complete()
                        } else if (error) {
                            subscriber.error(new Error(error))
                        }
                    })

                    store.dispatch(
                        AuthActions.refreshTokenRequest({ refreshToken })
                    )
                    return unsubscribe
                }).pipe(take(1), timeout(10000))
            }),
            catchError((error) =>
                throwError(
                    () =>
                        new Error(
                            error instanceof TimeoutError
                                ? 'Token refresh timed out'
                                : error.message
                        )
                )
            ),
            shareReplay(1)
        )

        // Clean up cache after it's done
        sharedRefresh$.subscribe({
            complete: () => (sharedRefresh$ = null),
            error: () => (sharedRefresh$ = null),
        })

        return sharedRefresh$
    },
}
