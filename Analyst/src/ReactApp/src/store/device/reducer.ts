import { createReducer } from '@reduxjs/toolkit'
import { UserDevice } from '../../entities/UserDevice'
import { DeviceActions } from './actions'

export interface DeviceState {
    device: UserDevice | null
    loading: boolean
    error: string | null
}

const initialState: DeviceState = {
    device: null,
    loading: false,
    error: null,
}

export const deviceReducer = createReducer(initialState, (builder) => {
    builder
        // Get User
        .addCase(DeviceActions.setDeviceRequest, (state) => {
            state.loading = true
            state.error = null
        })
        .addCase(DeviceActions.setDeviceSuccess, (state, action) => {
            state.device = action.payload
            state.loading = false
            state.error = null
        })
        .addCase(DeviceActions.setDeviceFailure, (state, action) => {
            state.loading = false
            state.error = action.payload.message
        })
})
