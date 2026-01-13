import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvCliTiposHistoricosesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvCliTiposHistoricoses(input: GetAdvCliTiposHistoricosesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advCliTiposHistoricoses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-cli-tipos-historicos", {
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

export function useAllAdvCliTiposHistoricoses() {
  return useQuery({
    queryKey: ["advCliTiposHistoricoses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-cli-tipos-historicos", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvCliTiposHistoricos(id: string) {
  return useQuery({
    queryKey: ["advCliTiposHistoricos", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-cli-tipos-historicos/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvCliTiposHistoricos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-cli-tipos-historicos", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advCliTiposHistoricoses"] });
    },
  });
}

export function useUpdateAdvCliTiposHistoricos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-cli-tipos-historicos/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advCliTiposHistoricoses"] });
      queryClient.invalidateQueries({ queryKey: ["advCliTiposHistoricos", data.id] });
    },
  });
}

export function useDeleteAdvCliTiposHistoricos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-cli-tipos-historicos/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advCliTiposHistoricoses"] });
    },
  });
}


