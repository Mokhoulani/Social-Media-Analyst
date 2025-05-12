import { AuthState } from './service'

export const authSelectors = {
    selectAccessToken: (state: { auth: AuthState }) => state.auth.accessToken,
    selectRefreshToken: (state: { auth: AuthState }) => state.auth.refreshToken,
    selectIsAuthenticated: (state: { auth: AuthState }) =>
        state.auth.isAuthenticated,
    selectLoading: (state: { auth: AuthState }) => state.auth.loading,
    selectError: (state: { auth: AuthState }) => state.auth.error,
}
