import { getStoredRefreshToken } from '../../utils/jwt-utils'
import { store } from '../store'
import { AuthActions } from './actions'
import { authSelectors } from './selectors'

/**
 * AuthFacade provides a simplified interface for components to interact with auth state
 * This abstracts away the Redux implementation details
 */
export const AuthFacade = {
    /**
     * Login with email and password
     */
    login: (email: string, password: string) => {
        store.dispatch(AuthActions.loginRequest({ email, password }))
    },

    /**
     * Sign up a new user
     */
    signUp: (
        firstName: string,
        lastName: string,
        email: string,
        password: string
    ) => {
        store.dispatch(
            AuthActions.signUpRequest({ firstName, lastName, email, password })
        )
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

    refreshTokenInterCeptor: () => {
        const refreshToken = getStoredRefreshToken()
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
}
