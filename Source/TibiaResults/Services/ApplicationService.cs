﻿using TibiaResults.Consts;
using TibiaResults.Helpers;
using TibiaResults.Interfaces;
using TibiaResults.Models;

namespace TibiaResults.Services
{
    internal class ApplicationService : IApplicationService
    {
        private readonly IResultFormatter _resultFormatter;
        private readonly IHighscoreRetrievalService _highscoreRetrievalService;
        private readonly ILevelTrackingService _levelTrackingService;
        private readonly IResultComputingService _resultComputingService;

        public ApplicationService(
            IResultFormatter resultFormatter,
            IHighscoreRetrievalService highscoreRetrievalService,
            ILevelTrackingService levelTrackingService,
            IResultComputingService resultComputingService)
        {
            _resultFormatter = resultFormatter;
            _highscoreRetrievalService = highscoreRetrievalService;
            _levelTrackingService = levelTrackingService;
            _resultComputingService = resultComputingService;
        }

        public async Task<string> RunAsync()
        {
            var result = await GetResultAsync();
            var formattedResult = _resultFormatter.FormatResult(result);

            return formattedResult;
        }

        private async Task<CategoryResult> GetCategoryResultAsync(Category category, ILevelTracker levelTracker)
        {
            var oldHighscore = await _highscoreRetrievalService.GetOldHighscoreAsync(category.Identifier);
            var newHighscore = await _highscoreRetrievalService.GetNewHighscoreAsync(category.Identifier);

            if (category == Categories.Experience)
            {
                return _resultComputingService.ComputeExperienceCategoryResult(oldHighscore, newHighscore, levelTracker);
            }

            _levelTrackingService.UpdateLevelTracker(levelTracker, oldHighscore, newHighscore);

            return _resultComputingService.ComputeCategoryResult(oldHighscore, newHighscore);
        }

        private async Task<IResult> GetResultAsync()
        {
            var result = Result.CreateNew();
            var levelTracker = LevelTracker.CreateEmpty();

            foreach (var category in CategoryHelper.GetCategories().OrderBy(c => c == Categories.Experience))
            {
                var categoryResult = await GetCategoryResultAsync(category, levelTracker);

                result.Add(category, categoryResult);
            }

            return result;
        }
    }
}
