import { z } from 'zod'
import { UsageSummarySchema } from './UsageSummary'
import { UserSocialMediaUsageSchema } from './UserSocialMediaUsage'
import { UserUsageGoalSchema } from './UserUsageGoal'

export const SocialMediaPlatformSchema = z.object({
    id: z.number().optional(),
    name: z.string().min(1, { message: 'Platform name is required' }),
    iconUrl: z
        .string()
        .url({ message: 'Invalid icon URL' })
        .or(z.literal(''))
        .optional(),

    usageRecords: z.array(UserSocialMediaUsageSchema).optional(),
    usageSummaries: z.array(UsageSummarySchema).optional(),
    usageGoals: z.array(UserUsageGoalSchema).optional(),
})

export type SocialMediaPlatform = z.infer<typeof SocialMediaPlatformSchema>

export function createSocialMediaPlatform(data: unknown): SocialMediaPlatform {
    return SocialMediaPlatformSchema.parse(data)
}

export const SocialMediaPlatformUpdateSchema =
    SocialMediaPlatformSchema.partial()

export type SocialMediaPlatformUpdate = z.infer<
    typeof SocialMediaPlatformUpdateSchema
>

export function updateSocialMediaPlatform(
    platform: SocialMediaPlatform,
    updates: Partial<SocialMediaPlatformUpdate>
): SocialMediaPlatform {
    const merged = { ...platform, ...updates }
    return SocialMediaPlatformSchema.parse(merged)
}
