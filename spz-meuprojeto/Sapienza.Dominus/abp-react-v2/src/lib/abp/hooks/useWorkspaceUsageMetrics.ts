import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetWorkspaceUsageMetricsInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useWorkspaceUsageMetrics(input: GetWorkspaceUsageMetricsInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["workspaceUsageMetrics", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/workspace-usage-metric", {
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

export function useAllWorkspaceUsageMetrics() {
  return useQuery({
    queryKey: ["workspaceUsageMetrics", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/workspace-usage-metric", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useWorkspaceUsageMetric(id: string) {
  return useQuery({
    queryKey: ["workspaceUsageMetric", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/workspace-usage-metric/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateWorkspaceUsageMetric() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/workspace-usage-metric", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["workspaceUsageMetrics"] });
    },
  });
}

export function useUpdateWorkspaceUsageMetric() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/workspace-usage-metric/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["workspaceUsageMetrics"] });
      queryClient.invalidateQueries({ queryKey: ["workspaceUsageMetric", data.id] });
    },
  });
}

export function useDeleteWorkspaceUsageMetric() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/workspace-usage-metric/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["workspaceUsageMetrics"] });
    },
  });
}


