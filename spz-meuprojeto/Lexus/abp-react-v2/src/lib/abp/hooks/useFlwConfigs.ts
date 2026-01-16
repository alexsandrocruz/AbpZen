import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetFlwConfigsInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useFlwConfigs(input: GetFlwConfigsInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["flwConfigs", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/flw-config", {
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

export function useAllFlwConfigs() {
  return useQuery({
    queryKey: ["flwConfigs", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/flw-config", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useFlwConfig(id: string) {
  return useQuery({
    queryKey: ["flwConfig", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/flw-config/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateFlwConfig() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/flw-config", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["flwConfigs"] });
    },
  });
}

export function useUpdateFlwConfig() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/flw-config/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["flwConfigs"] });
      queryClient.invalidateQueries({ queryKey: ["flwConfig", data.id] });
    },
  });
}

export function useDeleteFlwConfig() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/flw-config/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["flwConfigs"] });
    },
  });
}


