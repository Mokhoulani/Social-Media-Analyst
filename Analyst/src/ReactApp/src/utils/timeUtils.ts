export function parseHMSToSeconds(hms: string): number {
    const [hours = '0', minutes = '0', seconds = '0'] = hms.split(':')
    return +hours * 3600 + +minutes * 60 + +seconds
}

export function formatSecondsToHMS(totalSeconds: number): string {
    const hours = Math.floor(totalSeconds / 3600)
    const minutes = Math.floor((totalSeconds % 3600) / 60)
    const seconds = totalSeconds % 60

    return [hours, minutes, seconds]
        .map((n) => String(n).padStart(2, '0'))
        .join(':')
}
