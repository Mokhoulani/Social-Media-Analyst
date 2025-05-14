import { Epic, combineEpics } from 'redux-observable'
import { Action } from 'typesafe-actions'
import { authEpics } from './auth/epic'
import { RootState } from './root-reducer'
import { Dependencies } from './store'
import { userEpics } from './user/epic'

export const rootEpic: Epic<Action, Action, RootState, Dependencies> =
    combineEpics(
        ...(authEpics as Epic<Action, Action, RootState, Dependencies>[]),
        ...(userEpics as Epic<Action, Action, RootState, Dependencies>[])
    )
