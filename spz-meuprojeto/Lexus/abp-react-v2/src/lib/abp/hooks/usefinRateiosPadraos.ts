import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetFinRateiosPadraosInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useFinRateiosPadraos(input: GetFinRateiosPadraosInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["finRateiosPadraos", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fin-rateios-padrao", {
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

export function useAllFinRateiosPadraos() {
  return useQuery({
    queryKey: ["finRateiosPadraos", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fin-rateios-padrao", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useFinRateiosPadrao(id: string) {
  return useQuery({
    queryKey: ["finRateiosPadrao", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/fin-rateios-padrao/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateFinRateiosPadrao() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/fin-rateios-padrao", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["finRateiosPadraos"] });
    },
  });
}

export function useUpdateFinRateiosPadrao() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/fin-rateios-padrao/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["finRateiosPadraos"] });
      queryClient.invalidateQueries({ queryKey: ["finRateiosPadrao", data.id] });
    },
  });
}

export function useDeleteFinRateiosPadrao() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/fin-rateios-padrao/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["finRateiosPadraos"] });
    },
  });
}


