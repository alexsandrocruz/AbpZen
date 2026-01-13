import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvPreOrigensesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvPreOrigenses(input: GetAdvPreOrigensesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advPreOrigenses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-pre-origens", {
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

export function useAllAdvPreOrigenses() {
  return useQuery({
    queryKey: ["advPreOrigenses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-pre-origens", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvPreOrigens(id: string) {
  return useQuery({
    queryKey: ["advPreOrigens", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-pre-origens/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvPreOrigens() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-pre-origens", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advPreOrigenses"] });
    },
  });
}

export function useUpdateAdvPreOrigens() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-pre-origens/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advPreOrigenses"] });
      queryClient.invalidateQueries({ queryKey: ["advPreOrigens", data.id] });
    },
  });
}

export function useDeleteAdvPreOrigens() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-pre-origens/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advPreOrigenses"] });
    },
  });
}


