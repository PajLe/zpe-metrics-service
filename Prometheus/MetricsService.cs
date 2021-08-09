using System.Collections.Generic;
using Prometheus;
using ZurvitaPromoEngine.Services.Prometheus.Interfaces;

namespace ZurvitaPromoEngine.Services.Prometheus
{
    public class MetricsService : IMetricsService
    {
        private readonly Histogram PromotionRequestOrderSubtotalUsdHistogram;
        private readonly Histogram PromotionRequestOrderSubtotalMxnHistogram;
        private readonly Histogram PromotionRequestOrderSubtotalCadHistogram;
        private readonly Counter PromotionRequestWithPromoCounter;
        private readonly Counter PromotionRequestWithNoPromoCounter;
        private readonly Counter ApplyPromotionRequestWithPromoCounter;
        private readonly Counter ApplyPromotionRequestWithNoPromoCounter;
        private readonly Counter TalkableRewardWebhookAuthorizedCallCounter;
        private readonly Counter TalkableRewardWebhookUnauthorizedCallCounter;
        private readonly Counter TalkableAdvocatePromotionCreationCounter;
        private readonly Counter TalkableAdvocatePromotionCreationDeadlockErrorCounter;
        private readonly Counter PromoEngineErrorTotalCounter;

        public MetricsService()
        {
            PromotionRequestOrderSubtotalUsdHistogram = Metrics.CreateHistogram("promotion_request_order_subtotal_usd",
                "Ukupna cena korpe prosleđena u neki od endpoint-a 1, 2 ili 3, i to samo za markete sa USD valutom",
                new HistogramConfiguration
                {
                    Buckets = new double[] { 1, 10, 50, 100, 200, 500, 1000 },
                    StaticLabels = new Dictionary<string, string>
                    {
                        {"controller", "Promotion"}
                    },
                    LabelNames = new[] { "action", "market" }
                });

            PromotionRequestOrderSubtotalMxnHistogram = Metrics.CreateHistogram("promotion_request_order_subtotal_mxn",
                "Ukupna cena korpe prosleđena u neki od endpoint-a 1, 2 ili 3, i to samo za markete sa MXN valutom",
                new HistogramConfiguration
                {
                    Buckets = new double[] { 1, 500, 1000, 1500, 2000, 4000, 10000 },
                    StaticLabels = new Dictionary<string, string>
                    {
                        {"controller", "Promotion"}
                    },
                    LabelNames = new[] { "action", "market" }
                });

            PromotionRequestOrderSubtotalCadHistogram = Metrics.CreateHistogram("promotion_request_order_subtotal_cad",
                "Ukupna cena korpe prosleđena u neki od endpoint-a 1, 2 ili 3, i to samo za markete sa CAD valutom",
                new HistogramConfiguration
                {
                    Buckets = new double[] { 1, 10, 50, 100, 200, 500, 1000 },
                    StaticLabels = new Dictionary<string, string>
                    {
                        {"controller", "Promotion"}
                    },
                    LabelNames = new[] { "action", "market" }
                });

            PromotionRequestWithPromoCounter = Metrics.CreateCounter("promotion_request_with_promo_total",
                "Broj zahteva koji su vratili bar jednu promociju. Korisno za razlikovanje od „nekorisnih“ zahteva",
                new CounterConfiguration
                {
                    StaticLabels = new Dictionary<string, string>
                    {
                        {"controller", "Promotion"}
                    },
                    LabelNames = new[] { "site", "action" }
                });

            PromotionRequestWithNoPromoCounter = Metrics.CreateCounter("promotion_request_with_no_promo_total",
                "Broj zahteva koji nisu vratili ni jednu promociju. Korisno za razlikovanje od „korisnih“ zahteva",
                new CounterConfiguration
                {
                    StaticLabels = new Dictionary<string, string>
                    {
                        {"controller", "Promotion"}
                    },
                    LabelNames = new[] { "site", "action" }
                });

            ApplyPromotionRequestWithPromoCounter = Metrics.CreateCounter("apply_promotion_request_with_promo_total",
                "Broj zahteva ka endpoint-u 6 (za beleženje iskorišćenih promocija), koji sadrže bar jednu iskorišćenu promociju");

            ApplyPromotionRequestWithNoPromoCounter = Metrics.CreateCounter("apply_promotion_request_with_no_promo_total",
                "Broj zahteva ka endpoint-u 6 (za beleženje iskorišćenih promocija), koji ne sadrže ni jednu iskorišćenu promociju");

            TalkableRewardWebhookAuthorizedCallCounter = Metrics.CreateCounter("talkable_reward_webhook_authorized_call_total",
                "Broj autorizovanih zahteva ka endpoint-u 5 (za generisanje Talkable promocija)",
                new CounterConfiguration
                {
                    LabelNames = new[] { "reason" }
                });

            TalkableRewardWebhookUnauthorizedCallCounter = Metrics.CreateCounter("talkable_reward_webhook_unauthorized_call_total",
                "Broj neautorizovanih zahteva ka endpoint-u 5 (za generisanje Talkable promocija)");

            TalkableAdvocatePromotionCreationCounter = Metrics.CreateCounter("talkable_advocate_promotion_creation_count_total",
                "Broj pokušaja za kreiranje advocate promocija",
                new CounterConfiguration
                {
                    LabelNames = new[] { "status" }
                });

            TalkableAdvocatePromotionCreationDeadlockErrorCounter = Metrics.CreateCounter("talkable_advocate_promotion_creation_deadlock_error_total",
                "Broj grešaka koje su se javile zbog deadlock-a između SQL transakcija");

            PromoEngineErrorTotalCounter = Metrics.CreateCounter("promo_engine_error_count_total",
                "Broj grešaka svih značajnih endpoint-a",
                new CounterConfiguration
                {
                    LabelNames = new[] { "controller", "action" }
                });
        }

        public void ObservePromotionRequestOrderSubtotalUsdHistogram(double value, string action, string market)
        {
            PromotionRequestOrderSubtotalUsdHistogram.WithLabels(action, market).Observe(value);
        }

        public void ObservePromotionRequestOrderSubtotalMxnHistogram(double value, string action, string market)
        {
            PromotionRequestOrderSubtotalMxnHistogram.WithLabels(action, market).Observe(value);
        }

        public void ObservePromotionRequestOrderSubtotalCadHistogram(double value, string action, string market)
        {
            PromotionRequestOrderSubtotalCadHistogram.WithLabels(action, market).Observe(value);
        }

        public void IncreasePromotionRequestWithPromoCounter(string site, string action)
        {
            PromotionRequestWithPromoCounter.WithLabels(site, action).Inc();
        }

        public void IncreasePromotionRequestWithNoPromoCounter(string site, string action)
        {
            PromotionRequestWithNoPromoCounter.WithLabels(site, action).Inc();
        }

        public void IncreaseApplyPromotionRequestWithPromoCounter()
        {
            ApplyPromotionRequestWithPromoCounter.Inc();
        }

        public void IncreaseApplyPromotionRequestWithNoPromoCounter()
        {
            ApplyPromotionRequestWithNoPromoCounter.Inc();
        }

        public void IncreaseTalkableRewardWebhookAuthorizedCallCounter(string reason)
        {
            TalkableRewardWebhookAuthorizedCallCounter.WithLabels(reason).Inc();
        }

        public void IncreaseTalkableRewardWebhookUnauthorizedCallCounter()
        {
            TalkableRewardWebhookUnauthorizedCallCounter.Inc();
        }

        public void IncreaseTalkableAdvocatePromotionCreationCounter(string status)
        {
            TalkableAdvocatePromotionCreationCounter.WithLabels(status).Inc();
        }

        public void IncreaseTalkableAdvocatePromotionCreationDeadlockErrorCounter()
        {
            TalkableAdvocatePromotionCreationDeadlockErrorCounter.Inc();
        }

        public void IncreasePromoEngineErrorTotalCounter(string controller, string action)
        {
            PromoEngineErrorTotalCounter.WithLabels(controller, action).Inc();
        }

        public void ObservePromotionRequestOrderSubtotalHistogram(double subtotalValue, string action, string market)
        {
            string marketUpper = market.ToUpperInvariant();
            switch (marketUpper)
            {
                case "US":
                case "PR":
                    ObservePromotionRequestOrderSubtotalUsdHistogram(
                        subtotalValue,
                        action,
                        marketUpper);
                    break;
                case "CA":
                    ObservePromotionRequestOrderSubtotalCadHistogram(
                        subtotalValue,
                        action,
                        marketUpper);
                    break;
                case "MX":
                    ObservePromotionRequestOrderSubtotalMxnHistogram(
                        subtotalValue,
                        action,
                        marketUpper);
                    break;
            }
        }
    }
}
