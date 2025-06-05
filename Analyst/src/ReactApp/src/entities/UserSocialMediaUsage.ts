import { z } from 'zod'

export const UserSocialMediaUsageSchema = z.object({
    id: z.number().optional(),
    userId: z.string().uuid({ message: 'Invalid user ID' }).optional(),
    platformId: z.number().optional(),
    StartTime: z.string().min(1, { message: 'Start time is required' }),
    EndTime: z.string().min(1, { message: 'End time is required' }).optional(),
})

export type UserSocialMediaUsage = z.infer<typeof UserSocialMediaUsageSchema>

export function createUserSocialMediaUsage(
    data: unknown
): UserSocialMediaUsage {
    return UserSocialMediaUsageSchema.parse(data)
}

export const UserSocialMediaUsageUpdateSchema =
    UserSocialMediaUsageSchema.partial()

export type UserSocialMediaUsageUpdate = z.infer<
    typeof UserSocialMediaUsageUpdateSchema
>

export function updateUserSocialMediaUsage(
    usage: UserSocialMediaUsage,
    updates: Partial<UserSocialMediaUsageUpdate>
): UserSocialMediaUsage {
    const updated = { ...usage, ...updates }
    return UserSocialMediaUsageSchema.parse(updated)
}
