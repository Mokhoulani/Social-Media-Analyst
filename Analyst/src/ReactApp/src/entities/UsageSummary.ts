export interface UsageSummary {
    id: string
    userId: string
    platformId: number
    totalSeconds: number
    period: 'Daily' | 'Weekly' | 'Monthly'
    date: string
}
