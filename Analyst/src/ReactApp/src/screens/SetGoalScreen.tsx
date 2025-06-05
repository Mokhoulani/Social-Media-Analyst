import React, { useCallback, useEffect, useState } from 'react'
import { ActivityIndicator, Button, FlatList, Text, View } from 'react-native'
import { Card, Surface, TouchableRipple } from 'react-native-paper'
import Toast from 'react-native-root-toast'
import { useSelector } from 'react-redux'
import { CreateGoalDialog } from '../components/CreateGoalDialog'
import { UpdateGoalDialog } from '../components/UpdateGoalDialog'
import { UserUsageGoal } from '../entities/UserUsageGoal'
import { GoalActions } from '../store/goal/actions'
import { GoalPayload } from '../store/goal/service'
import { useAppDispatch, useAppSelector } from '../store/hook'
import { PlatformActions } from '../store/platform/actions'
import { userSelectors } from '../store/user/selectors'
import { useStyles } from '../themes/useStyles'

export default function SetGoalScreen() {
    const dispatch = useAppDispatch()
    const goals = useAppSelector((state) => state.goal.goals)
    const loading = useAppSelector((state) => state.goal.loading)
    const user = useSelector(userSelectors.selectUser)
    const platforms = useAppSelector((state) => state.platforms.platforms)
    const styles = useStyles()

    const [showDialog, setShowDialog] = useState(false)
    const [goalToUpdate, setGoalToUpdate] = useState<UserUsageGoal | null>(null)
    const [refreshing, setRefreshing] = useState(false)

    const handleSubmit = (data: UserUsageGoal) => {
        dispatch(GoalActions.setGoalRequest(data))
        setShowDialog(false)
    }

    useEffect(() => {
        dispatch(PlatformActions.getPlatformsRequest())
    }, [dispatch])

    useEffect(() => {
        if (user?.id) {
            const payload: GoalPayload = { userId: user.id }

            dispatch(GoalActions.getGoalsRequest(payload))
        } else {
            console.log('User ID not available yet')
        }
    }, [dispatch, user])

    const handleUpdate = (goal: UserUsageGoal) => {
        dispatch(GoalActions.setGoalRequest(goal))
        setShowDialog(false)
    }

    const onRefresh = useCallback(async () => {
        if (!user?.id) return

        setRefreshing(true)
        dispatch(GoalActions.getGoalsRequest({ userId: user.id }))
        setRefreshing(false)

        Toast.show('Goals updated', {
            duration: Toast.durations.SHORT,
            position: Toast.positions.BOTTOM,
        })
    }, [dispatch, user])

    if (loading && !refreshing) {
        return <ActivityIndicator style={{ marginTop: 10 }} />
    }

    return (
        <View style={{ flex: 1, padding: 16 }}>
            <FlatList
                data={goals}
                keyExtractor={(item) => item.id?.toString() ?? ''}
                refreshing={refreshing}
                onRefresh={onRefresh}
                renderItem={({ item }) => (
                    <Surface style={styles.surface}>
                        <TouchableRipple onPress={() => setGoalToUpdate(item)}>
                            <Card.Title
                                title={`${item.dailyLimit} min`}
                                subtitle="Daily Limit"
                                right={() => (
                                    <Text style={styles.updateText}>
                                        Update
                                    </Text>
                                )}
                            />
                        </TouchableRipple>
                    </Surface>
                )}
                ListEmptyComponent={
                    <Text style={styles.emptyText}>No goals set yet.</Text>
                }
                contentContainerStyle={
                    goals.length === 0 && styles.emptyListContainer
                }
            />

            <Button title="Add Goal" onPress={() => setShowDialog(true)} />

            {platforms.length > 0 && (
                <UpdateGoalDialog
                    visible={goalToUpdate !== null}
                    goal={goalToUpdate}
                    onClose={() => setGoalToUpdate(null)}
                    onSubmit={(data) => {
                        handleUpdate(data)
                        setGoalToUpdate(null)
                    }}
                    platforms={platforms}
                />
            )}

            {platforms.length > 0 && (
                <CreateGoalDialog
                    visible={showDialog}
                    onClose={() => setShowDialog(false)}
                    onSubmit={handleSubmit}
                    platforms={platforms}
                />
            )}
        </View>
    )
}
