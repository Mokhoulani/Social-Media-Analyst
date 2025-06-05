import { createAction } from '@reduxjs/toolkit'
import { UserDevice } from '../../entities/UserDevice'
import { Error } from '../../types/types'

export interface UserDevicePyload {
    deviceId: string
    deviceToken: string
}

export const DeviceActions = {
    getDeviceRequest: createAction('device/getDeviceRequest'),
    getDeviceSuccess: createAction<UserDevice>('device/getDeviceSuccess'),
    getDeviceFailure: createAction<Error>('device/getDeviceFailure'),

    setDeviceRequest: createAction<UserDevicePyload>('device/setDeviceRequest'),
    setDeviceSuccess: createAction<UserDevice>('device/setDeviceSuccess'),
    setDeviceFailure: createAction<Error>('device/setDeviceFailure'),
}

export type DeviceActionTypes =
    | ReturnType<typeof DeviceActions.getDeviceRequest>
    | ReturnType<typeof DeviceActions.getDeviceSuccess>
    | ReturnType<typeof DeviceActions.getDeviceFailure>
    | ReturnType<typeof DeviceActions.setDeviceRequest>
    | ReturnType<typeof DeviceActions.setDeviceSuccess>
    | ReturnType<typeof DeviceActions.setDeviceFailure>
