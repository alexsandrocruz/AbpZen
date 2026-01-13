import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvVerTiposesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvVerTiposes(input: GetAdvVerTiposesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advVerTiposes", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-ver-tipos", {
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

export function useAllAdvVerTiposes() {
  return useQuery({
    queryKey: ["advVerTiposes", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-ver-tipos", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvVerTipos(id: string) {
  return useQuery({
    queryKey: ["advVerTipos", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-ver-tipos/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvVerTipos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-ver-tipos", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advVerTiposes"] });
    },
  });
}

export function useUpdateAdvVerTipos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-ver-tipos/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advVerTiposes"] });
      queryClient.invalidateQueries({ queryKey: ["advVerTipos", data.id] });
    },
  });
}

export function useDeleteAdvVerTipos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-ver-tipos/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advVerTiposes"] });
    },
  });
}


