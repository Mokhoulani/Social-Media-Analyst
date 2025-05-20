import {
    catchError,
    Observable,
    take,
    throwError,
    timeout,
    TimeoutError,
} from 'rxjs'
import { User } from '../../entities/user'
import { store } from '../store'
import { UserActions } from './actions'
import { userSelectors } from './selectors'

/**
 * AuthFacade provides a simplified interface for components to interact with auth state
 * This abstracts away the Redux implementation details
 */

export const UserFacade = {
    getUser: (): Observable<User> => {
        return new Observable<User>((subscriber) => {
            const currentState = store.getState()
            const currentUser = userSelectors.selectUser(currentState)

            if (currentUser) {
                subscriber.next(currentUser)
                subscriber.complete()
                return
            }

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
            )
        )
    },
}
