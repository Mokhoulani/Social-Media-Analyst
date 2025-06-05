import { Action } from '@reduxjs/toolkit'
import { Epic, ofType } from 'redux-observable'
import { of } from 'rxjs'
import { catchError, map, mergeMap } from 'rxjs/operators'
import { UserDeviceSchema } from '../../entities/UserDevice'
import { RootState } from '../root.reducer'
import { Dependencies } from '../store'
import { DeviceActions } from './actions'
import { DeviceService } from './service'

export const createOrUpdateDeviceEpic: Epic<
    Action,
    Action,
    RootState,
    Dependencies
> = (action$) =>
    action$.pipe(
        ofType(DeviceActions.setDeviceRequest.type),
        mergeMap((action) => {
            const { payload } = action as ReturnType<
                typeof DeviceActions.setDeviceRequest
            >
            return DeviceService.createOrUpdateDevice(payload).pipe(
                map((response) => {
                    const result = UserDeviceSchema.safeParse(response)
                    if (result.success) {
                        return DeviceActions.setDeviceSuccess(result.data)
                    } else {
                        return DeviceActions.setDeviceFailure({
                            message: 'Invalid response schema',
                        })
                    }
                }),
                catchError((error) => {
                    const errorMessage = error?.message || 'Unknown error'
                    return of(
                        DeviceActions.setDeviceFailure({
                            message: errorMessage,
                        })
                    )
                })
            )
        })
    )

export const deviceEpics = [createOrUpdateDeviceEpic]
