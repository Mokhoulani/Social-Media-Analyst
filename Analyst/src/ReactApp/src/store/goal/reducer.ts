import { createReducer } from '@reduxjs/toolkit'
import { UserUsageGoal } from '../../entities/UserUsageGoal'
import { GoalActions } from './actions'

export interface GoalState {
    goals: UserUsageGoal[]
    loading: boolean
    error: string | null
}

const initialState: GoalState = {
    goals: [],
    loading: false,
    error: null,
}

export const goalReducer = createReducer(initialState, (builder) => {
    builder
        .addCase(GoalActions.getGoalsRequest, (state) => {
            state.loading = true
            state.error = null
        })
        .addCase(GoalActions.getGoalsSuccess, (state, action) => {
            state.goals = action.payload
            state.loading = false
        })
        .addCase(GoalActions.getGoalsFailure, (state, action) => {
            state.loading = false
            state.error = action.payload.message
        })
        .addCase(GoalActions.setGoalRequest, (state) => {
            state.loading = true
            state.error = null
        })
        .addCase(GoalActions.setGoalSuccess, (state, action) => {
            const index = state.goals.findIndex(
                (goal) => goal.platformId === action.payload.platformId
            )
            if (index !== -1) {
                state.goals[index] = action.payload
            } else {
                state.goals.push(action.payload)
            }
            state.loading = false
        })
        .addCase(GoalActions.setGoalFailure, (state, action) => {
            state.loading = false
            state.error = action.payload.message
        })
})
