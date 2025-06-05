import { createReducer } from '@reduxjs/toolkit'
import { User } from '../../entities/user'
import { UserActions } from './actions'

export interface UserState {
    user: User | null
    loading: boolean
    error: string | null
}

const initialState: UserState = {
    user: null,
    loading: false,
    error: null,
}

export const userReducer = createReducer(initialState, (builder) => {
    builder
        // Get User
        .addCase(UserActions.getUserRequest, (state) => {
            state.loading = true
            state.error = null
        })
        .addCase(UserActions.getUserSuccess, (state, action) => {
            state.user = action.payload
            state.loading = false
            state.error = null
        })
        .addCase(UserActions.getUserFailure, (state, action) => {
            state.loading = false
            state.error = action.payload.message
        })
})
