using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZurvitaPromoEngine.Services.Prometheus.Interfaces
{
    public interface IMetricsService
    {
        void ObservePromotionRequestOrderSubtotalUsdHistogram(double value, string action, string market);
        void ObservePromotionRequestOrderSubtotalMxnHistogram(double value, string action, string market);
        void ObservePromotionRequestOrderSubtotalCadHistogram(double value, string action, string market);
        void IncreasePromotionRequestWithPromoCounter(string site, string action);
        void IncreasePromotionRequestWithNoPromoCounter(string site, string action);
        void IncreaseApplyPromotionRequestWithPromoCounter();
        void IncreaseApplyPromotionRequestWithNoPromoCounter();
        void IncreaseTalkableRewardWebhookAuthorizedCallCounter(string reason);
        void IncreaseTalkableRewardWebhookUnauthorizedCallCounter();
        void IncreaseTalkableAdvocatePromotionCreationCounter(string status);
        void IncreaseTalkableAdvocatePromotionCreationDeadlockErrorCounter();
        void IncreasePromoEngineErrorTotalCounter(string controller, string action);

        // generalized methods
        void ObservePromotionRequestOrderSubtotalHistogram(double value, string action, string market);
    }
}
