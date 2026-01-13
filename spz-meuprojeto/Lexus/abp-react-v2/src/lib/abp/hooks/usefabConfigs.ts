import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetFabConfigsInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useFabConfigs(input: GetFabConfigsInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["fabConfigs", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fab-config", {
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

export function useAllFabConfigs() {
  return useQuery({
    queryKey: ["fabConfigs", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fab-config", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useFabConfig(id: string) {
  return useQuery({
    queryKey: ["fabConfig", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/fab-config/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateFabConfig() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/fab-config", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["fabConfigs"] });
    },
  });
}

export function useUpdateFabConfig() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/fab-config/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["fabConfigs"] });
      queryClient.invalidateQueries({ queryKey: ["fabConfig", data.id] });
    },
  });
}

export function useDeleteFabConfig() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/fab-config/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["fabConfigs"] });
    },
  });
}


