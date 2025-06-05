import { z } from 'zod'
import { UsageSummarySchema } from '../UsageSummary'
import { UserSocialMediaUsageSchema } from '../UserSocialMediaUsage'
import { UserUsageGoalSchema } from '../UserUsageGoal'

// Define schema
export const SocialMediaPlatformSchema = z.object({
    id: z.number().optional(),
    name: z.string().min(1, { message: 'Name is required' }),
    iconUrl: z.string().url({ message: 'Invalid icon URL' }),
    usageRecords: z.array(UserSocialMediaUsageSchema).optional(),
    usageSummaries: z.array(UsageSummarySchema).optional(),
    usageGoals: z.array(UserUsageGoalSchema).optional(),
})

// Export type
export type SocialMediaPlatform = z.infer<typeof SocialMediaPlatformSchema>

// Factory function
export function createSocialMediaPlatform(
    id: number,
    name: string,
    iconUrl: string
): SocialMediaPlatform {
    return SocialMediaPlatformSchema.parse({
        id,
        name,
        iconUrl,
        usageGoals: [],
        usageRecords: [],
        usageSummaries: [],
    })
}
