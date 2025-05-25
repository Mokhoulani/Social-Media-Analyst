import { Action } from '@reduxjs/toolkit'
import { Epic, ofType } from 'redux-observable'
import { of } from 'rxjs'
import { catchError, map, mergeMap } from 'rxjs/operators'
import { UserUsageGoalSchema } from '../../entities/UserUsageGoal'
import { RootState } from '../root.reducer'
import { Dependencies } from '../store'
import { GoalActions } from './actions'
import { GoalService } from './service'

export const getUsageGoalsEpic: Epic<
    Action,
    Action,
    RootState,
    Dependencies
> = (action$) =>
    action$.pipe(
        ofType(GoalActions.getGoalsRequest.type),
        mergeMap((action) =>
            GoalService.getGoals(
                (action as ReturnType<typeof GoalActions.getGoalsRequest>)
                    .payload
            ).pipe(
                // pass payload here
                map((response) => {
                    const validated = response.map((item) =>
                        UserUsageGoalSchema.parse(item)
                    )
                    return GoalActions.getGoalsSuccess(validated)
                }),
                catchError((error) => {
                    const errorMessage = error?.message || 'Unknown error'
                    return of(
                        GoalActions.getGoalsFailure({
                            message: errorMessage,
                        })
                    )
                })
            )
        )
    )

export const setUsageGoalEpic: Epic<Action, Action, RootState, Dependencies> = (
    action$
) =>
    action$.pipe(
        ofType(GoalActions.setGoalRequest.type),
        mergeMap((action) =>
            GoalService.setGoal(
                (action as ReturnType<typeof GoalActions.setGoalRequest>)
                    .payload
            ).pipe(
                map((response) => {
                    const parsed = UserUsageGoalSchema.parse(response)
                    return GoalActions.setGoalSuccess(parsed)
                }),
                catchError((error) => {
                    const errorMessage = error?.message || 'Unknown error'
                    return of(
                        GoalActions.setGoalFailure({
                            message: errorMessage,
                        })
                    )
                })
            )
        )
    )

export const goalEpics = [getUsageGoalsEpic, setUsageGoalEpic]
