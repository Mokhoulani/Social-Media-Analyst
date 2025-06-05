import { createReducer } from '@reduxjs/toolkit'
import { SocialMediaPlatform } from '../../entities/SocialMediaPlatform'
import { PlatformActions } from './actions'

export interface PlatformState {
    platforms: SocialMediaPlatform[]
    loading: boolean
    error: string | null
}

const initialState: PlatformState = {
    platforms: [],
    loading: false,
    error: null,
}

export const platformReducer = createReducer(initialState, (builder) => {
    builder
        .addCase(PlatformActions.getPlatformsRequest, (state) => {
            state.loading = true
            state.error = null
        })
        .addCase(PlatformActions.getPlatformsSuccess, (state, action) => {
            state.platforms = action.payload
            state.loading = false
        })
        .addCase(PlatformActions.getPlatformsFailure, (state, action) => {
            state.loading = false
            state.error = action.payload.message
        })
})
