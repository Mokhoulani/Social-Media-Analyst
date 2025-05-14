import { store } from '../store'
import { UserActions } from './actions'

/**
 * AuthFacade provides a simplified interface for components to interact with auth state
 * This abstracts away the Redux implementation details
 */
export const UserFacade = {
    /**
     * Get the current user data
     * This will dispatch the `getUserRequest` action to fetch user data
     * @returns void (as we're dispatching an action and don't expect a direct result here)
     */
    getUser: (): void => {
        store.dispatch(UserActions.getUserRequest())
    },
}
