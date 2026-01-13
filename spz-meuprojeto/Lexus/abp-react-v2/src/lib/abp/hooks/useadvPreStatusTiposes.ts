import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvPreStatusTiposesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvPreStatusTiposes(input: GetAdvPreStatusTiposesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advPreStatusTiposes", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-pre-status-tipos", {
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

export function useAllAdvPreStatusTiposes() {
  return useQuery({
    queryKey: ["advPreStatusTiposes", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-pre-status-tipos", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvPreStatusTipos(id: string) {
  return useQuery({
    queryKey: ["advPreStatusTipos", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-pre-status-tipos/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvPreStatusTipos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-pre-status-tipos", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advPreStatusTiposes"] });
    },
  });
}

export function useUpdateAdvPreStatusTipos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-pre-status-tipos/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advPreStatusTiposes"] });
      queryClient.invalidateQueries({ queryKey: ["advPreStatusTipos", data.id] });
    },
  });
}

export function useDeleteAdvPreStatusTipos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-pre-status-tipos/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advPreStatusTiposes"] });
    },
  });
}


