import { UsageSummary } from './UsageSummary'
import { UserSocialMediaUsage } from './UserSocialMediaUsage'
import { UserUsageGoal } from './UserUsageGoal '

export interface SocialMediaPlatform {
    id: number
    name: string
    iconUrl: string

    usageRecords?: UserSocialMediaUsage[]
    usageSummaries?: UsageSummary[]
    usageGoals?: UserUsageGoal[]
}
