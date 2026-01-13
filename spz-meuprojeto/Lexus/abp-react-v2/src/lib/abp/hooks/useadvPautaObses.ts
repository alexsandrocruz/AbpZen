import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvPautaObsesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvPautaObses(input: GetAdvPautaObsesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advPautaObses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-pauta-obs", {
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

export function useAllAdvPautaObses() {
  return useQuery({
    queryKey: ["advPautaObses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-pauta-obs", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvPautaObs(id: string) {
  return useQuery({
    queryKey: ["advPautaObs", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-pauta-obs/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvPautaObs() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-pauta-obs", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advPautaObses"] });
    },
  });
}

export function useUpdateAdvPautaObs() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-pauta-obs/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advPautaObses"] });
      queryClient.invalidateQueries({ queryKey: ["advPautaObs", data.id] });
    },
  });
}

export function useDeleteAdvPautaObs() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-pauta-obs/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advPautaObses"] });
    },
  });
}


