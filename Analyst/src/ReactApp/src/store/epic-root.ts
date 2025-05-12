import { combineEpics } from 'redux-observable'
import { loginEpic } from './auth/epic'

export const rootEpic = combineEpics(loginEpic)
