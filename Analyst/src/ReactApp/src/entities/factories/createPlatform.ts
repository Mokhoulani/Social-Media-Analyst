import { SocialMediaPlatform } from '../SocialMediaPlatform'

export const createSocialMediaPlatform = (
    id: number,
    name: string,
    iconUrl: string
): SocialMediaPlatform => ({
    id,
    name,
    iconUrl,
    usageGoals: [],
    usageRecords: [],
    usageSummaries: [],
})
