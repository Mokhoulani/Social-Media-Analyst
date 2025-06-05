import { createAction } from '@reduxjs/toolkit'
import { UserSocialMediaUsage } from '../../entities/UserSocialMediaUsage'
import { Error } from '../../types/types'
import { UsagePayload } from './service'

export const UsageActions = {
    getUsageRequest: createAction<UsagePayload>('usage/getUsageRequest'),
    getUsageSuccess: createAction<UserSocialMediaUsage[]>(
        'usage/getUsageSuccess'
    ),
    getUsgaeFailure: createAction<Error>('usage/getUsgaeFailure'),

    setUsageRequest: createAction<UserSocialMediaUsage>(
        'usage/setUsageRequest'
    ),
    setUsageSuccess: createAction<UserSocialMediaUsage>(
        'usage/setUsageSuccess'
    ),
    setUsageFailure: createAction<Error>('usage/setUsageFailure'),
}

export type DeviceActionTypes =
    | ReturnType<typeof UsageActions.getUsageRequest>
    | ReturnType<typeof UsageActions.getUsageSuccess>
    | ReturnType<typeof UsageActions.getUsgaeFailure>
    | ReturnType<typeof UsageActions.setUsageRequest>
    | ReturnType<typeof UsageActions.setUsageSuccess>
    | ReturnType<typeof UsageActions.setUsageFailure>
