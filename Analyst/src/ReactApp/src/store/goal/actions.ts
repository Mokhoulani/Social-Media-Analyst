import { createAction } from '@reduxjs/toolkit'
import { UserUsageGoal } from '../../entities/UserUsageGoal'
import { GoalError, GoalPayload } from './service'

export const GoalActions = {
    getGoalsRequest: createAction<GoalPayload>('goal/getGoalsRequest'),
    getGoalsSuccess: createAction<UserUsageGoal[]>('goal/getGoalsSuccess'),
    getGoalsFailure: createAction<GoalError>('goal/getGoalsFailure'),

    setGoalRequest: createAction<UserUsageGoal>('goal/setGoalRequest'),
    setGoalSuccess: createAction<UserUsageGoal>('goal/setGoalSuccess'),
    setGoalFailure: createAction<GoalError>('goal/setGoalFailure'),
}

export type UserUsageGoalActionTypes =
    | ReturnType<typeof GoalActions.getGoalsRequest>
    | ReturnType<typeof GoalActions.getGoalsSuccess>
    | ReturnType<typeof GoalActions.getGoalsFailure>
    | ReturnType<typeof GoalActions.setGoalRequest>
    | ReturnType<typeof GoalActions.setGoalSuccess>
    | ReturnType<typeof GoalActions.setGoalFailure>
