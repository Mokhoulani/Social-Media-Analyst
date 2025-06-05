import { createReducer } from '@reduxjs/toolkit'
import { UserSocialMediaUsage } from '../../entities/UserSocialMediaUsage'
import { UsageActions } from './actions'

export interface UsageState {
    usages: UserSocialMediaUsage[]
    loading: boolean
    error: string | null
}

const initialState: UsageState = {
    usages: [],
    loading: false,
    error: null,
}

export const usageReducer = createReducer(initialState, (builder) => {
    builder
        .addCase(UsageActions.getUsageRequest, (state) => {
            state.loading = true
            state.error = null
        })
        .addCase(UsageActions.getUsageSuccess, (state, action) => {
            state.usages = action.payload
            state.loading = false
        })
        .addCase(UsageActions.getUsgaeFailure, (state, action) => {
            state.loading = false
            state.error = action.payload.message
        })
        .addCase(UsageActions.setUsageRequest, (state) => {
            state.loading = true
            state.error = null
        })
        .addCase(UsageActions.setUsageSuccess, (state, action) => {
            const index = state.usages.findIndex(
                (usage) => usage.platformId === action.payload.platformId
            )
            if (index !== -1) {
                state.usages[index] = action.payload
            } else {
                state.usages.push(action.payload)
            }
            state.loading = false
        })
        .addCase(UsageActions.setUsageFailure, (state, action) => {
            state.loading = false
            state.error = action.payload.message
        })
})
