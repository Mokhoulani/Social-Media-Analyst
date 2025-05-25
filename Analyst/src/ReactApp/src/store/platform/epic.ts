import { Action } from '@reduxjs/toolkit'
import { Epic, ofType } from 'redux-observable'
import { of } from 'rxjs'
import { catchError, map, mergeMap } from 'rxjs/operators'
import { z } from 'zod'
import { SocialMediaPlatformSchema } from '../../entities/SocialMediaPlatform'
import { RootState } from '../root.reducer'
import { Dependencies } from '../store'
import { PlatformActions } from './actions'
import { PlatformService } from './service'

export const getPlatformsEpic: Epic<Action, Action, RootState, Dependencies> = (
    action$
) =>
    action$.pipe(
        ofType(PlatformActions.getPlatformsRequest.type),
        mergeMap(() =>
            PlatformService.getPlatforms().pipe(
                map((response) => {
                    const platforms = z
                        .array(SocialMediaPlatformSchema)
                        .safeParse(response)
                    if (platforms.success) {
                        return PlatformActions.getPlatformsSuccess(
                            platforms.data
                        )
                    } else {
                        return PlatformActions.getPlatformsFailure({
                            message: 'Invalid schema from server',
                        })
                    }
                }),
                catchError((error) =>
                    of(
                        PlatformActions.getPlatformsFailure({
                            message: error?.message ?? 'Unknown error',
                        })
                    )
                )
            )
        )
    )

export const platformEpics = [getPlatformsEpic]
