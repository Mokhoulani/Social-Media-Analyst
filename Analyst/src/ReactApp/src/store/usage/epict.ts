import { Action } from '@reduxjs/toolkit'
import { Epic, ofType } from 'redux-observable'
import { of } from 'rxjs'
import { catchError, map, mergeMap } from 'rxjs/operators'
import { UserSocialMediaUsageSchema } from '../../entities/UserSocialMediaUsage'
import { RootState } from '../root.reducer'
import { Dependencies } from '../store'
import { UsageActions } from './actions'
import { UsgaeService } from './service'

export const getUsageGoalsEpic: Epic<
    Action,
    Action,
    RootState,
    Dependencies
> = (action$) =>
    action$.pipe(
        ofType(UsageActions.getUsageRequest.type),
        mergeMap((action) =>
            UsgaeService.getUsage(
                (action as ReturnType<typeof UsageActions.getUsageRequest>)
                    .payload
            ).pipe(
                // pass payload here
                map((response) => {
                    const validated = response.map((item) =>
                        UserSocialMediaUsageSchema.parse(item)
                    )
                    return UsageActions.getUsageSuccess(validated)
                }),
                catchError((error) => {
                    const errorMessage = error?.message || 'Unknown error'
                    return of(
                        UsageActions.getUsgaeFailure({
                            message: errorMessage,
                        })
                    )
                })
            )
        )
    )
export const setUsageGEpic: Epic<Action, Action, RootState, Dependencies> = (
    action$
) =>
    action$.pipe(
        ofType(UsageActions.setUsageRequest.type),
        mergeMap((action) => {
            const { payload } = action as ReturnType<
                typeof UsageActions.setUsageRequest
            >
            return UsgaeService.setUsage(payload).pipe(
                map((response) => {
                    const result =
                        UserSocialMediaUsageSchema.safeParse(response)
                    console.log('setUsageGEpic', result)
                    if (result.success) {
                        return UsageActions.setUsageSuccess(result.data)
                    } else {
                        return UsageActions.setUsageFailure({
                            message: 'Invalid response schema',
                        })
                    }
                }),
                catchError((error) => {
                    const errorMessage = error?.message || 'Unknown error'
                    return of(
                        UsageActions.setUsageFailure({
                            message: errorMessage,
                        })
                    )
                })
            )
        })
    )

export const usageEpics = [getUsageGoalsEpic, setUsageGEpic]
