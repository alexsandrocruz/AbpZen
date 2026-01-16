import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetFinRateiosesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useFinRateioses(input: GetFinRateiosesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["finRateioses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fin-rateios", {
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

export function useAllFinRateioses() {
  return useQuery({
    queryKey: ["finRateioses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fin-rateios", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useFinRateios(id: string) {
  return useQuery({
    queryKey: ["finRateios", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/fin-rateios/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateFinRateios() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/fin-rateios", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["finRateioses"] });
    },
  });
}

export function useUpdateFinRateios() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/fin-rateios/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["finRateioses"] });
      queryClient.invalidateQueries({ queryKey: ["finRateios", data.id] });
    },
  });
}

export function useDeleteFinRateios() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/fin-rateios/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["finRateioses"] });
    },
  });
}


