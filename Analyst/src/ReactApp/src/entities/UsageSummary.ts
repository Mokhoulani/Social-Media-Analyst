import { z } from 'zod'

export const UsageSummarySchema = z.object({
    id: z.number().optional(),
    userId: z.string().uuid({ message: 'Invalid user ID' }).optional(),
    platformId: z.number().optional(),
    TotalDuration: z
        .string()
        .min(1, { message: 'Total duration is required' })
        .optional(),
    SummaryDate: z.string().min(1, { message: 'Summary date is required' }),
})

export type UsageSummary = z.infer<typeof UsageSummarySchema>

export function createUsageSummary(data: unknown): UsageSummary {
    return UsageSummarySchema.parse(data)
}

export const UsageSummaryUpdateSchema = UsageSummarySchema.partial()

export type UsageSummaryUpdate = z.infer<typeof UsageSummaryUpdateSchema>

export function updateUsageSummary(
    summary: UsageSummary,
    updates: Partial<UsageSummaryUpdate>
): UsageSummary {
    const merged = { ...summary, ...updates }
    return UsageSummarySchema.parse(merged)
}
