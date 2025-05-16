import { UserState } from './reducer'

export const userSelectors = {
    selectLoading: (state: { user: UserState }) => state.user.loading,
    selectError: (state: { user: UserState }) => state.user.error,
    selectUser: (state: { user: UserState }) => state.user.user,
    selectIsAuthenticated: (state: { user: UserState }) =>
        state.user.user !== null && state.user.error === null,
}
