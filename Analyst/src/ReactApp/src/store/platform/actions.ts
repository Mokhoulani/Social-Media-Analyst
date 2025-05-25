import { createAction } from '@reduxjs/toolkit'
import { SocialMediaPlatform } from '../../entities/SocialMediaPlatform'
import { PlatformError } from './service'

export const PlatformActions = {
    getPlatformsRequest: createAction('platform/getPlatformsRequest'),
    getPlatformsSuccess: createAction<SocialMediaPlatform[]>(
        'platform/getPlatformsSuccess'
    ),
    getPlatformsFailure: createAction<PlatformError>(
        'platform/getPlatformsFailure'
    ),
}

export type SocialMediaPlatformActionTypes =
    | ReturnType<typeof PlatformActions.getPlatformsRequest>
    | ReturnType<typeof PlatformActions.getPlatformsSuccess>
    | ReturnType<typeof PlatformActions.getPlatformsFailure>
