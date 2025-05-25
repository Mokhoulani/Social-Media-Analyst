import { Observable } from 'rxjs'
import { APIClient } from '../../api/apiClient'
import { UserUsageGoal } from '../../entities/UserUsageGoal'

export interface GoalError {
    message: string
}

export interface GoalPayload {
    userId: string
}

export const GoalService = {
    getGoals: (payload: GoalPayload): Observable<UserUsageGoal[]> => {
        return APIClient.post<UserUsageGoal[]>(
            '/UserUsageGoal/get-goals',
            payload
        )
    },
    setGoal: (goal: UserUsageGoal): Observable<UserUsageGoal> =>
        APIClient.post<UserUsageGoal>(
            '/UserUsageGoal/create-or-update-goal',
            goal
        ),
}
