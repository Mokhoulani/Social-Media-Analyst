import { createAction } from '@reduxjs/toolkit'
import { User } from '../../entities/user'
import { UserError } from './service'

export const UserActions = {
    getUserRequest: createAction('user/getUserRequest'),
    getUserSuccess: createAction<User>('user/getUserSuccess'),
    getUserFailure: createAction<UserError>('user/getUserFailure'),
}

export type UserActionTypes =
    | ReturnType<typeof UserActions.getUserRequest>
    | ReturnType<typeof UserActions.getUserSuccess>
    | ReturnType<typeof UserActions.getUserFailure>
