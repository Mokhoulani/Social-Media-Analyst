import React, { useEffect, useMemo } from 'react'
import { SafeAreaView, ScrollView, Text, View } from 'react-native'
import { ProgressBar } from 'react-native-paper'
import { useDispatch, useSelector } from 'react-redux'
import { RootState } from '../store/root.reducer'
import { UsageActions } from '../store/usage/actions'
import { userSelectors } from '../store/user/selectors'
import { useStyles, useThemeColors } from '../themes/useStyles'
import { formatSecondsToHMS, parseHMSToSeconds } from '../utils/timeUtils'

export default function DailyUsageScreen() {
    const styles = useStyles()
    const colors = useThemeColors()
    const user = useSelector(userSelectors.selectUser)

    const usageData = useSelector((state: RootState) => state.usage.usages)
    const goals = useSelector((state: RootState) => state.goal.goals)
    const platforms = useSelector(
        (state: RootState) => state.platforms.platforms
    )
    const dispatch = useDispatch()

    useEffect(() => {
        if (user?.id) {
            console.log('Fetching usage data for user:', user.id)
            dispatch(UsageActions.getUsageRequest({ userId: user.id }))
        } else {
            console.log('User ID not available yet')
        }
    }, [dispatch, user])

    const data = useMemo(() => {
        return usageData.map((usage) => {
            const goal = goals.find((g) => g.platformId === usage.platformId)
            const platform = platforms.find((p) => p.id === usage.platformId)
            const goalInSeconds = goal?.dailyLimit
                ? parseHMSToSeconds(goal.dailyLimit)
                : null

            return {
                platformName: platform?.name ?? 'Unknown',
                usageSeconds: usage.StartTime ?? 0,
                limitSeconds: goalInSeconds,
            }
        })
    }, [usageData, goals, platforms])

    return (
        <SafeAreaView style={styles.container}>
            <Text style={styles.title}>📊 Daily Social Media Usage</Text>

            <ScrollView contentContainerStyle={styles.scrollContent}>
                {data.map((item, index) => {
                    const { platformName, usageSeconds, limitSeconds } = item

                    const progress = limitSeconds
                        ? Math.min(Number(usageSeconds) / limitSeconds, 1)
                        : 0

                    const exceeded =
                        limitSeconds !== null &&
                        Number(usageSeconds) > limitSeconds

                    return (
                        <View key={index} style={styles.card}>
                            <Text style={styles.platformName}>
                                {platformName}
                            </Text>

                            <View style={styles.row}>
                                <Text style={styles.label}>Used:</Text>
                                <Text style={styles.value}>
                                    {formatSecondsToHMS(Number(usageSeconds))}
                                </Text>
                            </View>

                            {limitSeconds !== null && (
                                <View style={styles.row}>
                                    <Text style={styles.label}>Limit:</Text>
                                    <Text style={styles.value}>
                                        {formatSecondsToHMS(limitSeconds)}
                                    </Text>
                                </View>
                            )}

                            <ProgressBar
                                progress={progress}
                                color={exceeded ? colors.error : colors.primary}
                                style={styles.progressBar}
                            />

                            {exceeded && (
                                <Text style={styles.warning}>
                                    ⚠️ Limit Exceeded
                                </Text>
                            )}
                        </View>
                    )
                })}
            </ScrollView>
        </SafeAreaView>
    )
}
