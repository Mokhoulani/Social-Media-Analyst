import { createReducer } from '@reduxjs/toolkit'
import { AuthActions } from './actions' // or wherever you define them

export interface AuthState {
    accessToken: string | null
    refreshToken: string | null
    isAuthenticated: boolean
    loading: boolean
    error: string | null
}

const initialState: AuthState = {
    accessToken: null,
    refreshToken: null,
    isAuthenticated: false,
    loading: false,
    error: null,
}

export const authReducer = createReducer(initialState, (builder) => {
    builder
        // Login
        .addCase(AuthActions.loginRequest, (state) => {
            state.loading = true
            state.error = null
        })
        .addCase(AuthActions.loginSuccess, (state, action) => {
            state.accessToken = action.payload.accessToken
            state.refreshToken = action.payload.refreshToken
            state.isAuthenticated = true
            state.loading = false
            state.error = null
        })
        .addCase(AuthActions.loginFailure, (state, action) => {
            state.loading = false
            state.error = action.payload.message
        })

        // Sign Up
        .addCase(AuthActions.signUpRequest, (state) => {
            state.loading = true
            state.error = null
        })
        .addCase(AuthActions.signUpSuccess, (state, action) => {
            state.accessToken = action.payload.accessToken
            state.refreshToken = action.payload.refreshToken
            state.isAuthenticated = true
            state.loading = false
            state.error = null
        })
        .addCase(AuthActions.signUpFailure, (state, action) => {
            state.loading = false
            state.error = action.payload.message
        })

        // Refresh Token
        .addCase(AuthActions.refreshTokenRequest, (state) => {
            state.loading = true
            state.error = null
        })
        .addCase(AuthActions.refreshTokenSuccess, (state, action) => {
            state.accessToken = action.payload.accessToken
            state.refreshToken = action.payload.refreshToken
            state.isAuthenticated = true
            state.loading = false
        })
        .addCase(AuthActions.refreshTokenFailure, (state, action) => {
            state.loading = false
            state.error = action.payload.message
        })

        // Logout
        .addCase(AuthActions.logout, (state) => {
            state.loading = true
        })
        .addCase(AuthActions.logoutComplete, (state) => {
            state.accessToken = null
            state.refreshToken = null
            state.isAuthenticated = false
            state.loading = false
            state.error = null
        })
        .addCase(AuthActions.logoutError, (state, action) => {
            state.loading = false
            state.error = action.payload.message
        })
})
