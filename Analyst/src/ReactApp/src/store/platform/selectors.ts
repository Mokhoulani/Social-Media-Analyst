import { createSelector } from 'reselect'
import { PlatformState } from './reducer'

const selectPlatformState = (state: { platform: PlatformState }) =>
    state.platform

export const platformSelectors = {
    selectPlatforms: createSelector(
        [selectPlatformState],
        (platform) => platform?.platforms
    ),
    selectLoading: createSelector(
        [selectPlatformState],
        (platform) => platform.loading
    ),
    selectError: createSelector(
        [selectPlatformState],
        (platform) => platform.error
    ),
}
