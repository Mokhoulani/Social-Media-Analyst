import { Action } from '@reduxjs/toolkit'
import { Epic, ofType } from 'redux-observable'
import { of } from 'rxjs'
import { catchError, map, mergeMap } from 'rxjs/operators'
import { UserSchema } from '../../entities/user'
import { RootState } from '../root.reducer'
import { Dependencies } from '../store'
import { UserActions } from './actions'
import { UserService } from './service'

export const getUserEpic: Epic<Action, Action, RootState, Dependencies> = (
    action$
) =>
    action$.pipe(
        ofType(UserActions.getUserRequest.type),
        mergeMap(() =>
            UserService.getUser().pipe(
                map((response) => {
                    const result = UserSchema.safeParse(response)
                    if (result.success) {
                        return UserActions.getUserSuccess(result.data)
                    } else {
                        return UserActions.getUserFailure({
                            message: 'Invalid response schema',
                        })
                    }
                }),
                catchError((error) => {
                    // Check if error is valid before accessing the message property
                    const errorMessage = error?.message || 'Unknown error'
                    return of(
                        UserActions.getUserFailure({
                            message: errorMessage,
                        })
                    )
                })
            )
        )
    )

export const userEpics = [getUserEpic]
