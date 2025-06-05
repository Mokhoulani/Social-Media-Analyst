import {
    catchError,
    from,
    Observable,
    shareReplay,
    take,
    throwError,
    timeout,
    TimeoutError,
} from 'rxjs'
import { User } from '../../entities/user'
import { store } from '../store'
import { UserActions } from './actions'
import { userSelectors } from './selectors'

let userRequest$: Observable<User> | null = null

export const UserFacade = {
    getUser: (): Observable<User> => {
        const currentState = store.getState()
        const currentUser = userSelectors.selectUser(currentState)

        if (currentUser) {
            return from(Promise.resolve(currentUser))
        }

        // Reuse the observable if already created
        if (!userRequest$) {
            userRequest$ = new Observable<User>((subscriber) => {
                const unsubscribe = store.subscribe(() => {
                    const state = store.getState()
                    const user = userSelectors.selectUser(state)
                    const error = userSelectors.selectError(state)

                    if (user) {
                        subscriber.next(user)
                        subscriber.complete()
                    } else if (error) {
                        subscriber.error(new Error(error))
                    }
                })

                store.dispatch(UserActions.getUserRequest())

                return unsubscribe
            }).pipe(
                take(1),
                timeout(10000),
                catchError((error) =>
                    throwError(
                        () =>
                            new Error(
                                error instanceof TimeoutError
                                    ? 'User data loading timed out'
                                    : error.message
                            )
                    )
                ),
                shareReplay(1) // Cache the result for subsequent subscribers
            )
            userRequest$.subscribe({
                complete: () => (userRequest$ = null),
                error: () => (userRequest$ = null),
            })
        }

        return userRequest$
    },
}
