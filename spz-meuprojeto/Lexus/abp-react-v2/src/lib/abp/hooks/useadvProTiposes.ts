import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvProTiposesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvProTiposes(input: GetAdvProTiposesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advProTiposes", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-pro-tipos", {
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

export function useAllAdvProTiposes() {
  return useQuery({
    queryKey: ["advProTiposes", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-pro-tipos", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvProTipos(id: string) {
  return useQuery({
    queryKey: ["advProTipos", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-pro-tipos/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvProTipos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-pro-tipos", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advProTiposes"] });
    },
  });
}

export function useUpdateAdvProTipos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-pro-tipos/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advProTiposes"] });
      queryClient.invalidateQueries({ queryKey: ["advProTipos", data.id] });
    },
  });
}

export function useDeleteAdvProTipos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-pro-tipos/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advProTiposes"] });
    },
  });
}


