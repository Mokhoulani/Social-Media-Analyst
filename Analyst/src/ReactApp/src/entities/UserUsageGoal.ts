import { z } from 'zod'

export const UserUsageGoalSchema = z.object({
    id: z.number().optional(),
    userId: z.string().uuid().optional(),
    platformId: z
        .number()
        .refine((val) => val !== 0, { message: 'Platform must be selected' }),
    dailyLimit: z.string().regex(/^\d{2}:\d{2}:\d{2}$/, {
        message: 'Must be in hh:mm:ss format',
    }),

    createdOnUtc: z.string().datetime().optional(),
})

export type UserUsageGoal = z.infer<typeof UserUsageGoalSchema>

export function createUserUsageGoal(data: unknown): UserUsageGoal {
    return UserUsageGoalSchema.parse(data)
}

export const UserUsageGoalUpdateSchema = UserUsageGoalSchema.partial()

export type UserUsageGoalUpdate = z.infer<typeof UserUsageGoalUpdateSchema>

export function updateUserUsageGoal(
    existing: UserUsageGoal,
    updates: Partial<UserUsageGoalUpdate>
): UserUsageGoal {
    const merged = { ...existing, ...updates }
    return UserUsageGoalSchema.parse(merged)
}

export function validateUserUsageGoal(data: unknown): UserUsageGoal {
    return UserUsageGoalSchema.parse(data)
}

export function validatePartialUserUsageGoal(
    data: unknown
): Partial<UserUsageGoal> {
    return UserUsageGoalUpdateSchema.parse(data)
}
