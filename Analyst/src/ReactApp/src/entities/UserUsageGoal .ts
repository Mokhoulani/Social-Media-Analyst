export interface UserUsageGoal {
    id: string
    userId: string
    platformId: number
    maxSecondsPerDay: number
    createdOnUtc: string
}
