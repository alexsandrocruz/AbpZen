import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetSiteVisitDailyStatsInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  siteId?: string;
  }

export function useSiteVisitDailyStats(input: GetSiteVisitDailyStatsInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["siteVisitDailyStats", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/site-visit-daily-stat", {
        params: {
          filter,
          skipCount,
          maxResultCount,
          ...rest,
        },
      });
      return response.data;
    },
  });
}

export function useAllSiteVisitDailyStats() {
  return useQuery({
    queryKey: ["siteVisitDailyStats", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/site-visit-daily-stat", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useSiteVisitDailyStat(id: string) {
  return useQuery({
    queryKey: ["siteVisitDailyStat", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/site-visit-daily-stat/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateSiteVisitDailyStat() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/site-visit-daily-stat", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["siteVisitDailyStats"] });
    },
  });
}

export function useUpdateSiteVisitDailyStat() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/site-visit-daily-stat/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["siteVisitDailyStats"] });
      queryClient.invalidateQueries({ queryKey: ["siteVisitDailyStat", data.id] });
    },
  });
}

export function useDeleteSiteVisitDailyStat() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/site-visit-daily-stat/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["siteVisitDailyStats"] });
    },
  });
}


