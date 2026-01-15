import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetLeadStageHistoriesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  leadId?: string;
  }

export function useLeadStageHistories(input: GetLeadStageHistoriesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["leadStageHistories", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/lead-stage-history", {
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

export function useAllLeadStageHistories() {
  return useQuery({
    queryKey: ["leadStageHistories", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/lead-stage-history", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useLeadStageHistory(id: string) {
  return useQuery({
    queryKey: ["leadStageHistory", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/lead-stage-history/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateLeadStageHistory() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/lead-stage-history", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["leadStageHistories"] });
    },
  });
}

export function useUpdateLeadStageHistory() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/lead-stage-history/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["leadStageHistories"] });
      queryClient.invalidateQueries({ queryKey: ["leadStageHistory", data.id] });
    },
  });
}

export function useDeleteLeadStageHistory() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/lead-stage-history/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["leadStageHistories"] });
    },
  });
}


